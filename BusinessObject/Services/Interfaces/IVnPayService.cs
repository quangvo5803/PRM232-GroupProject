﻿using DataAccess.Entities.Application;
using Microsoft.AspNetCore.Http;

namespace BusinessObject.Services.Interfaces
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel requestModel);
        VnPaymentResponseModel PaymentExecute(IQueryCollection query);
    }
}
