﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

}
