using agricultureAPI.Data;
using agricultureAPI.Data.Models;
using agricultureAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace agricultureAPI.Services
{
    public class DatabaseCropService : ICropService
    {
        private readonly CropDbContext context;

        private CropOutputModel ToOutputModel(Crop crop)
           => new CropOutputModel(crop.Id, crop.Title, crop.Description, crop.Duration);

        public DatabaseCropService(
            CropDbContext context)
        {
            this.context = context;
        }

        public async Task<CropOutputModel> Add(CropInputModel model)
        {
            var dbCrop = new Crop
            {
                Title = model.Title,
                Description = model.Description,
                Duration = model.Duration
            };
            context.Crops.Add(dbCrop);
            await context.SaveChangesAsync();
            return ToOutputModel(dbCrop);
        }

        public async Task<bool> Delete(int id)
        {
            return await context.Crops.Where(item => item.Id == id).ExecuteDeleteAsync() > 0;
        }

        public async Task<List<CropOutputModel>> GetAll()
        {
            return (await context.Crops.ToListAsync()).ConvertAll(ToOutputModel);
        }

        public async Task<CropOutputModel?> GetById(int id)
        {
            var dbCrop = await context.Crops.FirstOrDefaultAsync(item => item.Id == id);
            if (dbCrop is null) return null;
            return ToOutputModel(dbCrop);
        }

        public async Task<bool> Update(int id, CropInputModel item)
        {
            var dbCrop = await context.Crops.FirstOrDefaultAsync(item => item.Id == id);
            if (dbCrop is null) return false;
            dbCrop.Title = item.Title;
            dbCrop.Description = item.Description;
            dbCrop.Duration = item.Duration;

            context.Crops.Update(dbCrop);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
