﻿namespace Mango.Web.Models
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public object? Result { get; set; }

    }
}
