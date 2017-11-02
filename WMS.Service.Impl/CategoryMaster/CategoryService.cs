using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WMS.Repository;
using WIM.Core.Common.Validation;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WMS.Context;
using WMS.Entity.ItemManagement;
using WMS.Repository.Impl;
using WIM.Core.Context;
using System.Security.Principal;
using WMS.Common.ValueObject;

namespace WMS.Service
{
    public class CategoryService : ICategoryService
    {
        private IIdentity user { get; set; }
        public CategoryService(IIdentity identity)
        {
            user = identity;
        }

        public IEnumerable<CategoryDto> GetCategories()
        {
            IEnumerable<CategoryDto> categoryDtos;
            using (WMSDbContext Db = new WMSDbContext())
            {

            ICategoryRepository repo = new CategoryRepository(Db);
            IEnumerable<Category_MT> categorys = repo.Get();

            categoryDtos = categorys.Select(b => new CategoryDto()
            {
                CateIDSys = b.CateIDSys,
                CateID = b.CateID,
                CateName = b.CateName,
                IsActive = b.IsActive,
                ProjectIDSys = b.ProjectIDSys
            });
            }
            return categoryDtos;
        }

        public IEnumerable<CategoryDto> GetCategoriesByProjectID(int projectIDSys)
        {
            IEnumerable<CategoryDto> categoryDtos;
            using (WMSDbContext Db = new WMSDbContext())
            {

                ICategoryRepository repo = new CategoryRepository(Db);
                IEnumerable<Category_MT> categorys = repo.GetMany(c=>c.ProjectIDSys == projectIDSys && c.IsActive == true);
                categoryDtos = categorys.Select(b => new CategoryDto()
                {
                    CateIDSys = b.CateIDSys,
                    CateID = b.CateID,
                    CateName = b.CateName,
                    IsActive = b.IsActive,
                    ProjectIDSys = b.ProjectIDSys
                });
            }
            return categoryDtos;
        }

        public CategoryDto GetCategory(int id)
        {
            CategoryDto categoryDto;
            using (WMSDbContext Db = new WMSDbContext())
            {

                ICategoryRepository repo = new CategoryRepository(Db);
                Category_MT category = repo.GetByID(id);
                categoryDto = new CategoryDto();
                categoryDto.CateIDSys = category.CateIDSys;
                categoryDto.CateID = category.CateID;
                categoryDto.CateName = category.CateName;
                categoryDto.IsActive = category.IsActive;
                categoryDto.ProjectIDSys = category.ProjectIDSys;
            }
            return categoryDto;
        }

        public int CreateCategory(Category_MT category )
        {
            using (var scope = new TransactionScope()) {

                using (WMSDbContext Db = new WMSDbContext())
                {

                    ICategoryRepository repo = new CategoryRepository(Db);
                    category.CateID = Db.ProcGetNewID("CT");
                    try
                    {
                        repo.Insert(category);
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        HandleValidationException(e);
                    }
                    catch (DbUpdateException)
                    {
                        scope.Dispose();
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                        throw ex;
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
                        repo.Update(category);
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        HandleValidationException(e);
                    }
                    catch (DbUpdateException)
                    {
                        scope.Dispose();
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                        throw ex;
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
                        scope.Complete();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        scope.Dispose();
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                        throw ex;
                    }

                }
                return true;
            }
        }

        public void HandleValidationException(DbEntityValidationException ex)
        {
            foreach (var eve in ex.EntityValidationErrors)
            {
                foreach (var ve in eve.ValidationErrors)
                {
                    throw new ValidationException(ve.PropertyName, ve.ErrorMessage);
                }
            }
        }

    }
}
