using agricultureAPI.Models;

namespace agricultureAPI.Services
{
    public interface ICropService
    {
        Task<CropOutputModel> Add(CropInputModel model);

        Task<bool> Delete(int id);

        Task<List<CropOutputModel>> GetAll();

        Task<CropOutputModel?> GetById(int id);

        Task<bool> Update(int id, CropInputModel item);

    }
}