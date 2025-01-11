﻿using Domain.Enums;

namespace Domain.Models.Organization;

public class User : Person
{
    public ERole Role { get; set; }
    public Organization? Organization { get; set; }

}
