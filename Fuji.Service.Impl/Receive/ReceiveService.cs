using Fuji.Common.ValueObject;
using Fuji.Context;
using Fuji.Entity.ItemManagement;
using Fuji.Service.Receive;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

            List<InterfaceReceiveItem> receiveItems = new List<InterfaceReceiveItem>();

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
                    var serialNumber1 = group.ReceiveItems[0].SerialNumber; 
                    var serialNumber2 = group.ReceiveItems.Count > 1 ? group.ReceiveItems[1].SerialNumber : null;  
                    var serialNumber = new SerialNumber(_ProjectId, 0, 0, ++seq, serialNumber1, serialNumber2, null, group.Key.ItemGroup);
                    serialNumberList.Add(serialNumber);
                }

                var receiveItem = new InterfaceReceiveItem(
                    itemCode: groupIB.Key.ItemCode, 
                    scanCode: "", 
                    jAN: "",
                    qty: 1, 
                    unit: "ชิ้น",
                    smallestQty: 1, 
                    smallestUnit: "ชิ้น", 
                    cost: 0, 
                    price: 0, 
                    currency: "THB", 
                    width: 0, 
                    height: 0, 
                    length: 0, 
                    weight: 0,
                    location: receive.Location, 
                    expireDate: null, 
                    manufacturingDate: null, 
                    bestBeforeDate: null, 
                    controlLevel1: groupIB.Key.BoxNumber, 
                    controlLevel2: "", 
                    controlLevel3: "", 
                    remark: "", 
                    serialNumbers: serialNumberList
               );

                receiveItems.Add(receiveItem);
            }

            var receiveCommand = new InterfaceReceiveCommand(
                receiveType: "Normal",
                receiveQualityType: "GOOD",
                receiveNo: receive.HeadID,
                poNo: receive.InvoiceNumber,
                remark: receive.Remark,
                poDate: receive.ReceivingDate,
                receiveDate: receive.ReceivingDate,
                supplierCode: "FUJI001",
                receiveItems: receiveItems
            );

            return receiveCommand;
        }

        public async Task<bool> Confirm2Stock(string headId)
        {
            var receive = GetReceived(headId);
            var receiveCommand = MapReceiveForInterface(receive);
            var stringData = JsonConvert.SerializeObject(receiveCommand);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");            
            var url = $"{ConfigurationManager.AppSettings["receiveServiceUrl"]}/interfaces/confirm2stock";
            var result = await UtilityHelper.PostAsync<int>(url, contentData, ConfigurationManager.AppSettings["identity"]);
            return result != 0;
        }
    }
}
