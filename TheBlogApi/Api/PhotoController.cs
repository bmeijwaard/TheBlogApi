using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Api.Base;
using TheBlogApi.Data.Services.Contracts;

namespace TheBlogApi.Api
{
    [Route("~/api/v1/photo")]
    public class PhotoController : BaseApiController    
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }
    }
}
