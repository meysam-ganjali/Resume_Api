using System.ComponentModel.DataAnnotations;

namespace Resume.SharedModel.Roles;

public class RoleDto : BaseDto {
    public string Name { get; set; }
}

public class CreateRole {
    [MaxLength(100, ErrorMessage = "تعداد کارامتر مجاز 100 می باشد")]
    [Required(ErrorMessage = "نام نقش را وارد نکرده اید")]
    public string Name { get; set; }
}
public class UpdateRole : CreateRole {
    public Guid Id { get; set; }
    public DateTime UpdateDate { get; set; }
    [MaxLength(100, ErrorMessage = "تعداد کارامتر مجاز 100 می باشد")]
    [Required(ErrorMessage = "نام نقش را وارد نکرده اید")]
    public string Name { get; set; }
}