using Shared.DTOs.Wishlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface IWishlistService
    {
        Task<WishlistToggleResponseDto> ToggleWishlistAsync(string userId, int courseId);
        Task<List<WishlistCourseDto>> GetMyWishlistAsync(string userId);
    }
}
