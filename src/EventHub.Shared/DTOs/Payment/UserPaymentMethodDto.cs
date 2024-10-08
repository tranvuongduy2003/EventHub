﻿using EventHub.Shared.DTOs.User;

namespace EventHub.Shared.DTOs.Payment;

public class UserPaymentMethodDto
{
    public Guid Id { get; set; }
    
    public string PaymentAccountNumber { get; set; } = string.Empty;

    public string? PaymentAccountQRCodeUrl { get; set; } = string.Empty;
    
    public string? CheckoutContent { get; set; } = string.Empty;
    
    public UserDto Author { get; set; } = null!;
    
    public PaymentMethodDto PaymentMethod { get; set; } = null!;
}