using Fuji.Common.ValueObject;
using Fuji.Context;
using Fuji.Entity.ItemManagement;
using Fuji.Service.Receive;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Service.Impl.StatusManagement;

namespace Fuji.Service.Impl.Receive
{
    public class ReceiveService : WIM.Core.Service.Impl.Service, IReceiveService
    {
        private const int _SUBMODULE_ID = 10;
        private string RECEIVED = StatusServiceStatic.GetStatusBySubmoduleIDAndStatusTitle<string>(_SUBMODULE_ID, FujiStatus.Received.GetValueEnum());
        private string _ProjectId = ConfigurationManager.AppSettings["projectId"];

        public ImportSerialHead GetReceived(string headId)
        {
            ImportSerialHead headItem;
            using (FujiDbContext Db = new FujiDbContext())
            {
                headItem = (from h in Db.ImportSerialHead
                            where h.HeadID == headId
                            && h.Status == RECEIVED
                            select h).FirstOrDefault();
                if (headItem == null)
                {
                    throw new NullReferenceException($"Head Number: {headId} Not Found!");
                }
                Db.Entry(headItem).Collection(c => c.ImportSerialDetail).Load();
            }
            return headItem;
        }

        public InterfaceReceiveCommand MapReceiveForInterface(ImportSerialHead receive)
        {

            var receiveGroupByItemCodeAndBoxNumbers = receive.ImportSerialDetail.GroupBy(
                i => new { i.ItemCode, i.BoxNumber },
                i => i,
                (key, g) => new { Key = key, ReceiveItems = g.ToList() });


            int seq = 0;

            List<ReceiveItem> receiveItems = new List<ReceiveItem>();

            var serialNumberList = new List<SerialNumber>();
            foreach (var groupIB in receiveGroupByItemCodeAndBoxNumbers)
            {
                serialNumberList.Clear();
                var receiveGroupByItemGroups = groupIB.ReceiveItems.GroupBy(
              i => new { i.ItemGroup },
              i => i,
              (key, g) => new { Key = key, ReceiveItems = g.ToList() });

                foreach (var group in receiveGroupByItemGroups)
                {
                    var serialNumber1 = group.ReceiveItems[0].SerialNumber; //group.ReceiveItems.FirstOrDefault(i => i.ItemGroup == group.Key.ItemGroup && i.ItemType == "1").SerialNumber;
                    var serialNumber2 = group.ReceiveItems.Count > 1 ? group.ReceiveItems[1].SerialNumber : null;  //group.ReceiveItems.FirstOrDefault(i => i.ItemGroup == group.Key.ItemGroup && i.ItemType == "2").SerialNumber;
                    var serialNumber = new SerialNumber(_ProjectId, 0, 0, ++seq, serialNumber1, serialNumber2, null, group.Key.ItemGroup);
                    serialNumberList.Add(serialNumber);

                }

                var receiveItem = new ReceiveItem(0, 0, 0, 0, groupIB.Key.ItemCode, null, null, 0, 0, 0, 0, 0, 0, 0
                , 0, 0, 0, 0, null, null, null, null, null, null, true, groupIB.Key.BoxNumber, false, null, false, null
                , true, serialNumberList, false, null, null, null, "", false);
                receiveItems.Add(receiveItem);
            }

            var receiveCommand = new InterfaceReceiveCommand(1, 57, 1, 1, receive.InvoiceNumber
                , receive.ReceivingDate, receive.ReceivingDate, "S001", "Sup1"
                , "Address 1", "Address 2", "Address 3", "Address 4", "Address 5"
                , "Address 1 EN", "Address 2 EN", "Address 3 EN", "Address 4 EN", "Address 5 EN"
                , "02256982", "10320", "", "", "", false, 0
                , null, null
                , receiveItems
                );
            return receiveCommand;
        }

        public async Task<bool> Confirm2Stock(string headId)
        {
            var receive = GetReceived(headId);
            var receiveCommand = MapReceiveForInterface(receive);
            var stringData = JsonConvert.SerializeObject(receiveCommand);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
            HttpClient client = await UtilityHelper.GetHttpClientForIdentityServerAsync(ConfigurationManager.AppSettings["identity"]);
            var url = $"{ConfigurationManager.AppSettings["receiveServiceUrl"]}/interfaces/confirm2stock";
            HttpResponseMessage response = await client.PostAsync(url, contentData);
            string body = await response.Content.ReadAsStringAsync();
            return true;
        }
    }
}
