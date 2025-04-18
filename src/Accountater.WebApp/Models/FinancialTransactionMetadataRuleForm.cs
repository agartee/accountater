﻿using Accountater.Domain.Models;

namespace Accountater.WebApp.Models
{
    public record FinancialTransactionMetadataRuleForm
    {
        public required FinancialTransactionMetadataRuleId Id { get; init; }
        public required string Name { get; set; }
        public required string Expression { get; set; }
        public required FinancialTransactionMetadataType MetadataType { get; set; }
        public required string MetadataValue { get; set; }
    }
}
