﻿using GoodNewsAggregator.Core.DTO;

namespace GoodNewsAggregator.WebAPI.Auth
{
    public class JwtAuthResult
    {
        public string AccessToken { get; set; }
        public RefreshTokenDto RefreshToken { get; set; }
    }
}