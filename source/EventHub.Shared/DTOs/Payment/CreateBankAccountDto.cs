﻿namespace EventHub.Shared.DTOs.Payment;

public class CreateBankAccountDto
{
    public string AccountHolderName { get; set; }

    public string AccountNumber { get; set; }
}