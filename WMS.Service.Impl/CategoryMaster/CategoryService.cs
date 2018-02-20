
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

namespace WMS.Service
{
    public class CategoryService : WIM.Core.Service.Impl.Service , ICategoryService
    {

        public CategoryService()
        {
            
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
                        Category_MT x = repo.Insert(category);
                         Db.SaveChanges();
                        scope.Complete();
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
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new ValidationException(e);
                    }
                    catch (DbUpdateException)
                    {
                        scope.Dispose();
                        throw new ValidationException(ErrorCode.E4012);

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
                        throw new ValidationException(ErrorEnum.E4017);
                    }

                }
                return true;
            }
        }
    }
}
