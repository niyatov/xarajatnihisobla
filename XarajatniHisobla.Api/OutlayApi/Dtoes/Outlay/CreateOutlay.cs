﻿namespace OutlayApi.Dtoes;

public class CreateOutlay
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Cost { get; set; }
    public int CategoryId { get; set; }
}