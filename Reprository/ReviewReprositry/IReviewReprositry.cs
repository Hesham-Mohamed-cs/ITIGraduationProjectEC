using GraduaitionProjectITI.Models.Entity;
using GraduaitionProjectITI.ViewModel;
using GraduaitionProjectITI.ViewModel.Review;

namespace GraduaitionProjectITI.Reprository.ReviewReprositry
{
    public interface IReviewReprositry
    {
        List<ReviewProductNameAndNoFReviewViewModel> GetAllProductThatHaveRevies();
        List<GetReviewsViewModel> GetReviewsOnSpecificProduct(int ProductId);

        void Insert(AddReviewDBViewModel addReviewDBViewModel);
    }
}
