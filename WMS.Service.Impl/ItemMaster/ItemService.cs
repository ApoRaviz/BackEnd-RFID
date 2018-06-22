using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WMS.Context;
using WMS.Entity.ItemManagement;
using WIM.Core.Repository.Impl;
using WMS.Repository;
using System.Security.Principal;
using WMS.Common.ValueObject;
using WMS.Repository.Impl;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;
using WMS.Repository.ItemManagement;
using WMS.Repository.Impl.ItemManagement;
using WMS.Entity.InspectionManagement;
using WIM.Core.Service.Impl;

namespace WMS.Service
{
    public class ItemService : WIM.Core.Service.Impl.Service, IItemService
    {
        public ItemService()
        {
        }

        public IEnumerable<ItemDto> GetItems()
        {
            IEnumerable<ItemDto> itemDtos;
            using (WMSDbContext Db = new WMSDbContext())
            {
                IItemRepository repo = new ItemRepository(Db);
                IEnumerable<Item_MT> items = repo.Get();
                itemDtos = Mapper.Map<IEnumerable<Item_MT>, IEnumerable<ItemDto>>(items);

            }
            return itemDtos;
        }

        public IEnumerable<AutocompleteItemDto> AutocompleteItem(string term)
        {
            IEnumerable<AutocompleteItemDto> autocompleteItemDto;
            using (WMSDbContext Db = new WMSDbContext())
            {
                IItemRepository repo = new ItemRepository(Db);
                autocompleteItemDto = repo.AutocompleteItem(term);
               
            }
            return autocompleteItemDto;
        }

        public ItemDto GetItem(int id, string[] tableNames)
        {
            /*Item_MT item = (from i in db.Item_MT
                            where i.ItemIDSys == id && i.Active == 1
                            select i)
                            .Include(it => it.Project_MT)
                            .Include(it => it.Supplier_MT)
                            .Include(it => it.ItemUnitMapping.Select(s => s.Unit_MT))
                            .SingleOrDefault();*/

            //return Mapper.Map<Item_MT, ItemDto>(item);

            using (WMSDbContext Db = new WMSDbContext())
            {
                IItemRepository repo = new ItemRepository(Db);
                var query = repo.GetManyWithUnit(id);

                var inspect = query.ItemInspectMapping.Select(a => new Inspect_MT()
                {
                    InspectID = a.Inspect_MT.InspectID,
                    InspectIDSys = a.Inspect_MT.InspectIDSys
                }).ToList();
                var sending = Mapper.Map<Item_MT, ItemDto>(query);
                if(query.ItemSet_MT != null)
                sending.ItemSetName = query.ItemSet_MT.ItemSetName;
                if(query.Category_MT != null)
                {
                    sending.CateName = query.Category_MT.CateName;
                    if(query.Category_MT.Control_MT != null)
                    {
                        sending.ControlValue = query.Category_MT.Control_MT.ControlDetails.Select(a => new ControlValueDto()
                        {
                            Key = a.Key,
                            Unit = a.Unit,
                            Value = a.Value
                        }).ToList();

                        if(query.Category_MT.MainCategory != null)
                        {
                            if(query.Category_MT.MainCategory.Control_MT != null)
                            {
                                foreach(var control in query.Category_MT.MainCategory.Control_MT.ControlDetails)
                                {
                                    if(sending.ControlValue.Any(s => s.Key != control.Key))
                                    {
                                        //ControlValueDto value = new ControlValueDto() { }
                                        //sending.ControlValue.Add();
                                    }
                                }
                            }
                        }
                    }
                }
                if(sending.SupIDSys != null)
                {
                    sending.SupName = Db.Supplier_MT.Where(a => a.SupIDSys.ToString() == sending.SupIDSys).Select(b => b.CompName).FirstOrDefault();
                }
                sending.SpareFields = Db.ProcGetSpareFieldsByTableAndRefID(sending.ProjectIDSys, "Item_MT",sending.ItemIDSys);
                sending.ItemInspectMapping.Clear();
                foreach (var data in inspect)
                {
                    sending.ItemInspectMapping.Add(data);
                }
                return sending;
            }
        }

        public int CreateItem(Item_MT item)
        {
            using (var scope = new TransactionScope())
            {
                Item_MT itemresponse = new Item_MT();
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IItemRepository repo = new ItemRepository(Db);
                        IItemUnitRepository repoUnit = new ItemUnitRepository(Db);
                        IItemInspectRepository repoInspect = new ItemInspectRepository(Db);
                        itemresponse = repo.Insert(item);
                        Db.SaveChanges();
                        if (item.ItemUnitMapping != null)
                        {
                            foreach (var data in item.ItemUnitMapping)
                            {
                                data.ItemIDSys = itemresponse.ItemIDSys;
                                repoUnit.Insert(data);
                            }
                        }
                        if (item.ItemInspectMapping != null)
                        {
                            foreach (var data in item.ItemInspectMapping)
                            {
                                data.ItemIDSys = itemresponse.ItemIDSys;
                                repoInspect.Insert(data);
                            }
                        }
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                }
                return itemresponse.ItemIDSys;
            }
        }

        public int CreateItemGift(ItemGiftDto item)
        {
            using (var scope = new TransactionScope())
            {
                Item_MT itemresponse = new Item_MT();
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IItemRepository repo = new ItemRepository(Db);
                        IItemUnitRepository repoUnit = new ItemUnitRepository(Db);
                        Item_MT itemForInsert = new CommonService().AutoMapper<Item_MT>(item);
                        itemresponse = repo.Insert(itemForInsert);
                        Db.SaveChanges();
                        ItemUnitMapping itemunitforinsert = new CommonService().AutoMapper<ItemUnitMapping>(item);
                        itemunitforinsert.ItemIDSys = itemresponse.ItemIDSys;
                        itemunitforinsert.Sequence = 0;
                        itemunitforinsert.QtyInParent = 1;
                        repoUnit.Insert(itemunitforinsert);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                }
                return itemresponse.ItemIDSys;
            }
        }

        public ItemUnitMapping CreateItemUnit(ItemUnitMapping itemunit)
        {
            using (var scope = new TransactionScope())
            {
                ItemUnitMapping itemresponse = new ItemUnitMapping();
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                       
                        IItemUnitRepository repoUnit = new ItemUnitRepository(Db);
                        itemunit.Sequence = 0;
                        itemunit.QtyInParent = 1;
                        itemresponse = repoUnit.Insert(itemunit);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                }
                return itemresponse;
            }
        }

        public bool UpdateItem(Item_MT item)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IItemRepository repo = new ItemRepository(Db);
                        IItemInspectRepository repoInspect = new ItemInspectRepository(Db);
                        IItemUnitRepository repoUnit = new ItemUnitRepository(Db);
                        ISpareFieldDetailRepository repoSparefd = new SpareFieldDetailRepository(Db);
                        repo.Update(item);
                        var listiteminspect = repoInspect.GetMany(a => a.ItemIDSys == item.ItemIDSys);
                        var listitemunit = repoUnit.GetMany(a => a.ItemIDSys == item.ItemIDSys);
                        Db.ItemInspectMapping.RemoveRange(listiteminspect);
                        Db.ItemUnitMapping.RemoveRange(listitemunit);
                        Db.SaveChanges();
                        if (item.ItemUnitMapping != null)
                        {
                            foreach (var data in item.ItemUnitMapping)
                            {
                                data.ItemIDSys = item.ItemIDSys;
                                repoUnit.Insert(data);
                            }
                        }
                        if (item.ItemInspectMapping != null)
                        {
                            foreach (var data in item.ItemInspectMapping)
                            {
                                data.ItemIDSys = item.ItemIDSys;
                                repoInspect.Insert(data);
                            }
                        }
                        
                        if (item.SpareFields != null) { 
                            repoSparefd.insertByDto(item.ItemIDSys,item.SpareFields);
                        }

                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                }
                return true;
            }
        }

        public bool DeleteItem(int id)
        {
            using (var scope = new TransactionScope())
            {

                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IItemRepository repo = new ItemRepository(Db);
                        repo.Delete(id);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    throw new AppValidationException(ErrorEnum.UPDATE_DATABASE_CONCURRENCY_PROBLEM);
                }
                return true;
            }
        }

        public bool DeleteItemUnit(ItemUnitMapping item)
        {
            using (var scope = new TransactionScope())
            {

                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IItemUnitRepository repo = new ItemUnitRepository(Db);
                        repo.Delete(item);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    throw new AppValidationException(ErrorEnum.UPDATE_DATABASE_CONCURRENCY_PROBLEM);
                }
                return true;
            }
        }

    }
}
