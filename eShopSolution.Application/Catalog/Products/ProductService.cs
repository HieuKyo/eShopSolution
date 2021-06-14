
using eShopSolution.Application.Common;
using eShopSolution.Data.Entities;
using eShopSolution.Data.Entities_Framework;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;

using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        //Cần 2 cái Class ManageProductService PublicProductService
        //Cải tiến button để đơn giản hơn
        //Tạo 1 constructor ManageProductService

        //Cần khai báo 1 biến context private để đón nó
        private readonly eShopDBContext _context;
        private readonly IStorageService _storageService;
        public ProductService(eShopDBContext context, IStorageService storageService)
        {
            //Cần đầu vào là 1 DbContext cái đã khai báo ở eShop.Data
            //Cần add reference từ tầng Data lên, qua cáu Dependencies
            _context = context;
            _storageService = storageService;
        }

        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            //Tạo mới product = Entity framework
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                //Ghi đè thằng DateTime luôn
                DateCreated = DateTime.Now,
                //Thêm Translation theo dạng cha con
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        //Thêm thông qua khoá ngoại
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        //Thêm bản ghi đúng bằng cái Language đó
                        LanguageId = request.LanguageId,
                    }
                }
            };
            //Sau khi thêm xong Add cái product này vào SaveChange
            //Save Image
            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                { new ProductImage()
                    {
                    Caption ="Thumbnail image",
                    DateCreated = DateTime.Now,
                    FileSize = request.ThumbnailImage.Length,
                    ImagePath = await this.SaveFile(request.ThumbnailImage),
                    IsDefault = true,
                    SortOrder = 1
                    }
                };
            }
            _context.Products.Add(product);
            //Do xài SaveChangesAsync nên phải đổi kiểu của int trong Create thành Task<int>
            //await nó, cho nó chạy background thay vì phải chờ, giảm thời gian chờ
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException($"Can't find a product: {productId}");

            var images = _context.ProductImages.Where(i => i.ProductId == productId);


            //Chúng ta sẽ foreach nó qua 1 loạt, nếu mà có 
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }
            _context.Products.Remove(product);
            //Còn tìm dc thì sẽ trả về số bản ghi Delete
            return await _context.SaveChangesAsync();
        }

        //Phương thức GetPaging
        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            //Dùng cách query trong LinQ dễ hơn
            //1. Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        where pt.Name.Contains(request.Keyword)
                        select new { p, pt, pic };

            //2. filter
            //Kiểm tra nếu keyword khác rỗng mới tìm
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }
            //Có bất cứ tìm kiếm nào
            if (request.CategoryIds.Count > 0)
            {
                query = query.Where(p => request.CategoryIds.Contains(p.pic.CategoryId));
            }

            //3. Paging
            //Bước này ta phải lấy ra dc totalRow lấy dc tổng số bản ghi
            int totalRow = await query.CountAsync();

            //Ví dụ: Ở bản ghi ở trang 1. 1 - 1 = 0, 0 * 10 = 0
            //trang 2. 2 - 1 = 1, 1 * 10 = 10 Bỏ qua 10 bản ghi đầu thì sẽ lấy trang số 2.
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();

            //4 Select and projection
            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pageResult;
        }

        public async Task<ProductViewModel> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            //FirstOrDefaultAsync lấy ra thằng đầu tiên, k có thì nó sẽ rỗng
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId
                                                                                            && x.LanguageId == languageId);
            var productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                DateCreated = product.DateCreated,
                Description = productTranslation != null ? productTranslation.Description : null,
                LanguageId = productTranslation.LanguageId,
                Details = productTranslation != null ? productTranslation.Details : null,
                Name = productTranslation != null ? productTranslation.Name : null,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                SeoAlias = productTranslation != null ? productTranslation.SeoAlias : null,
                SeoDescription = productTranslation != null ? productTranslation.SeoDescription : null,
                SeoTitle = productTranslation != null ? productTranslation.SeoTitle : null,
                Stock = product.Stock,
                ViewCount = product.ViewCount
            };
            return productViewModel;
        }

        public Task<ProductViewModel> GetById(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId)
                .Select(i => new ProductImageViewModel()
                {
                    Caption = i.Caption,
                    DateCreated = i.DateCreated,
                    FileSize = i.FileSize,
                    Id = i.Id,
                    ImagePath = i.ImagePath,
                    IsDefault = i.IsDefault,
                    ProductId = i.ProductId,
                    SortOrder = i.SortOrder,
                }).ToListAsync();
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            //Để update được thì ta update ProductTranslation
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id
                                        && x.LanguageId == request.LanguageId);
            //Nếu 1 trong 2 thằng null thì ta sẽ báo lỗi
            if (product == null || productTranslation == null) throw new eShopException($"Can't find a product with id : {request.Id}");
            //Nếu khác null thì sẽ
            productTranslation.Name = request.Name;
            productTranslation.SeoAlias = request.SeoAlias;
            productTranslation.SeoDescription = request.SeoDescription;
            productTranslation.SeoTitle = request.SeoTitle;
            productTranslation.Description = request.Description;
            productTranslation.Details = request.Details;
            //Sau khi update các thông tin trên xong thì chúng ta sẽ
            //Save Image
            if (request.ThumbnailImage != null)
            {
                //Ta sẽ k thêm mới ở đây nữa, mà ta sẽ kiểm tra
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    //Nếu có 1 thằng thumbnail rồi, mới update nó, update file khác
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.ProductImages.Update(thumbnailImage);
                }
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException($"Can't find a product with id : {productId}");
            product.Price = newPrice;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException($"Can't find a product with id : {productId}");
            product.Stock += addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }
        //SaveImage này nó có 1 hàm riêng
        private async Task<string> SaveFile(IFormFile file)
        {
            //Lấy ra fileName, get Extension
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            //Tạo ra 1 guid mới với 1 tên file
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            //Rồi Save vào 1 cái _storageService
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder
            };
            //Nếu trường hợp ImageFile khác null
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
            // tổng bản ghi afftected
            await _context.SaveChangesAsync();
            // Trả về productImage.Id để tý còn binding được
            return productImage.Id;
        }
        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new eShopException($"Cannot find an image with id {imageId} =='");
            //Nếu trường hợp ImageFile khác null
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            //Trả về tổng bản ghi afftected
            return await _context.SaveChangesAsync();
        }
        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new eShopException($"Cannot find an image with id {imageId} =='");
            _context.ProductImages.Remove(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        { 
            //find nó ra trước 
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null)
                throw new eShopException($"Cannot find an image with id {imageId} =='");
            var viewModel =  new ProductImageViewModel()
                {
                    Caption = image.Caption,
                    DateCreated = image.DateCreated,
                    FileSize = image.FileSize,
                    Id = image.Id,
                    ImagePath = image.ImagePath,
                    IsDefault = image.IsDefault,
                    ProductId = image.ProductId,
                    SortOrder = image.SortOrder,
                };
            return viewModel;
        }
        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };

            //2. filter
            //Có bất cứ tìm kiếm nào
            //Chấp nhận giá trị 0
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
            }

            //3. Paging
            //Tìm theo categoryId lấy ra được tổng số dòng để phân trang
            int totalRow = await query.CountAsync();

            //Ví dụ: Ở bản ghi ở trang 1. 1 - 1 = 0, 0 * 10 = 0
            //trang 2. 2 - 1 = 1, 1 * 10 = 10 Bỏ qua 10 bản ghi đầu thì sẽ lấy trang số 2.
            //Chỉ lấy ra sản phẩm với Category đó thôi, còn k thì thôi
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();

            //4 Select and projection
            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pageResult;
        }
    }
}
