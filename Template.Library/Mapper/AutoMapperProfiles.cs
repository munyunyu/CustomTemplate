using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Template.Library.Tables.User;
using AutoMapper;
using Template.Library.Views.System;
using Template.Library.ViewsModels.Profile;
using Template.Library.ViewsModels.System;

namespace Template.Library.Mapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ViewSystemUserRoles, SystemUserRolesViewModel>();
            //.ForMember(dst => dst.Id, src => src.MapFrom(src => src.Id))

            //CreateMap<TblMerchantEcommerce, EcommerceViewModel>()
            //    .ForMember(dst => dst.EcommercePlatform, src => src.MapFrom(src => (src.EcommercePlatform != null) ? src.EcommercePlatform.Name : ""))
            //    .ForMember(dst => dst.PosPlatform, src => src.MapFrom(src => (src.PosPlatform != null) ? src.PosPlatform.Name : ""));

            //mapping for ProfileViewModel and nested class
            CreateMap<TblProfile, ProfileViewModel>();
            //CreateMap<TblAgent, ProfileViewModel.ProfileViewAgent>();

            

            //CreateMap<TblMerchant, MerchantViewModel>();
            //CreateMap<TblProfile, MerchantViewModel.MerchantViewModelProfile>();
            //CreateMap<TblPayment, MerchantViewModel.MerchantViewModelPayments>();
            //CreateMap<TblChannel, MerchantViewModel.MerchantViewModelChannel>();
            //CreateMap<TblVideo, MerchantViewModel.MerchantViewModelVideo>();



        }

    }
}
