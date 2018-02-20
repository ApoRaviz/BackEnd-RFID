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
                    throw new ValidationException(e);
                }
                catch (DbUpdateException e)
                {
                    scope.Dispose();
                    throw new ValidationException(ErrorEnum.E4012);
                }
                return itemresponse.ItemIDSys;
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

                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new ValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    throw new ValidationException(ErrorEnum.E4012);
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
                    throw new ValidationException(ErrorEnum.E4017);
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
                    throw new ValidationException(ErrorEnum.E4017);
                }
                return true;
            }
        }

    }
}
