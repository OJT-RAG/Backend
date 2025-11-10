using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OJT_RAG.Repositories.Entities;

[Table("company")]
public partial class Company
{
    [Key]
    [Column("company_id")]
    public long CompanyId { get; set; }

    [Column("majorid")]
    public long? Majorid { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string? Name { get; set; }

    [Column("tax_code")]
    [StringLength(100)]
    public string? TaxCode { get; set; }

    [Column("address")]
    [StringLength(255)]
    public string? Address { get; set; }

    [Column("website")]
    [StringLength(255)]
    public string? Website { get; set; }

    [Column("contact_email")]
    [StringLength(255)]
    public string? ContactEmail { get; set; }

    [Column("phone")]
    [StringLength(50)]
    public string? Phone { get; set; }

    [Column("logo_url")]
    [StringLength(255)]
    public string? LogoUrl { get; set; }

    [Column("create_at", TypeName = "timestamp without time zone")]
    public DateTime? CreateAt { get; set; }

    [Column("update_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdateAt { get; set; }

    [Column("is_verified")]
    public bool? IsVerified { get; set; }

    [ForeignKey("Majorid")]
    [InverseProperty("Companies")]
    public virtual Major? Major { get; set; }

    [InverseProperty("Company")]
    public virtual ICollection<SemesterCompany> SemesterCompanies { get; set; } = new List<SemesterCompany>();

    [InverseProperty("Company")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
