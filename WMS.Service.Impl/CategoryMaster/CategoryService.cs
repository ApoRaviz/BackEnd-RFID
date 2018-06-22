
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Transactions;
using WMS.Repository;

using System.Data.Entity.Infrastructure;
using WMS.Context;
using WMS.Entity.ItemManagement;
using WMS.Common.ValueObject;
using WMS.Repository.Impl;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;
using WMS.Repository.ControlMaster;
using WMS.Repository.Impl.ControlMaster;
using WMS.Entity.ControlMaster;
using System;

namespace WMS.Service
{
    public class CategoryService : WIM.Core.Service.Impl.Service , ICategoryService
    {

        public CategoryService()
        {
            
        }

        public IEnumerable<Category_MT> GetCategories()
        {
            IEnumerable<Category_MT> categoryDtos;
            using (WMSDbContext Db = new WMSDbContext())
            {

            ICategoryRepository repo = new CategoryRepository(Db);
            categoryDtos = repo.Get();
            }
            return categoryDtos;
        }

        public IEnumerable<Category_MT> GetCategoriesByProjectID(int projectIDSys)
        {
            IEnumerable<Category_MT> categoryDtos;
            using (WMSDbContext Db = new WMSDbContext())
            {

                ICategoryRepository repo = new CategoryRepository(Db);
                categoryDtos = repo.GetMany(c=>c.ProjectIDSys == projectIDSys && c.IsActive == true);
                
            }
            return categoryDtos;
        }

        public Category_MT GetCategory(int id)
        {
            Category_MT categoryDto;
            using (WMSDbContext Db = new WMSDbContext())
            {

                ICategoryRepository repo = new CategoryRepository(Db);
                string[] include = { "Control_MT", "MainCategory" };
                categoryDto = repo.GetWithInclude(a => a.CateIDSys == id,include).SingleOrDefault();
            }
            return categoryDto;
        }

        public int CreateCategory(Category_MT category )
        {
            using (var scope = new TransactionScope()) {

                using (WMSDbContext Db = new WMSDbContext())
                {
                    IControlRepository controlrepo = new ControlRepository(Db);
                    ICategoryRepository repo = new CategoryRepository(Db);
                    category.CateID = Db.ProcGetNewID("CT");
                    try
                    {
                        if( category.Control_MT != null)
                        {
                            if (category.Control_MT.ControlDetails.Count > 0)
                            {
                                Control_MT control = new Control_MT();
                                control = controlrepo.Insert(category.Control_MT);
                                Db.SaveChanges();
                                category.ControlIDSys = control.ControlIDSys;
                            }
                        }
                        category.Control_MT = null;
                        category.MainCategory = null;
                        Category_MT x = repo.Insert(category);
                         Db.SaveChanges();
                        scope.Complete();
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
                }
            }
                return category.CateIDSys;
            
        }

        public bool UpdateCategory(Category_MT category)
        {
            using (var scope = new TransactionScope())
            {
                using (WMSDbContext Db = new WMSDbContext())
                {
                    ICategoryRepository repo = new CategoryRepository(Db);
                    try
                    {
                        category.MainCategory = null;
                        repo.Update(category);
                        Db.SaveChanges();
                        scope.Complete();
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
                }
                return true;
            }
        }

        public bool DeleteCategory(int id)
        {
            using (var scope = new TransactionScope())
            {
                using (WMSDbContext Db = new WMSDbContext())
                {

                    ICategoryRepository repo = new CategoryRepository(Db);
                    var existedCategory = repo.GetByID(id);
                    try
                    {
                        repo.Delete(existedCategory);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        scope.Dispose();
                        throw new AppValidationException(ErrorEnum.UPDATE_DATABASE_CONCURRENCY_PROBLEM);
                    }

                }
                return true;
            }
        }

        public IEnumerable<AutocompleteCategoryDto> AutocompleteCategory(string term)
        {
            IEnumerable<AutocompleteCategoryDto> autocompleteItemDto;
            using (WMSDbContext Db = new WMSDbContext())
            {
                ICategoryRepository repo = new CategoryRepository(Db);
                autocompleteItemDto = repo.AutocompleteCategory(term);

            }
            return autocompleteItemDto;
        }
    }
}
