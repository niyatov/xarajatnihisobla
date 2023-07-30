﻿using System.ComponentModel.DataAnnotations;

namespace OutlayApi.Dtoes;

public class CreateCategory
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Key { get; set; }
}
