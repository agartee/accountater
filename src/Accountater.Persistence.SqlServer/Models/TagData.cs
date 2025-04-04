﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accountater.Persistence.SqlServer.Models
{
    [Table(TableName)]
    public record TagData
    {
        public const string TableName = "Tag";

        public required Guid Id { get; set; }

        [MaxLength(200)]
        public required string Value { get; set; }

        public List<FinancialTransactionData> Transactions { get; set; } = new();
        public List<TagRuleData> TagRules { get; set; } = new();
    }
}
