﻿using RssReader.Domain.Common;

namespace RssReader.Domain.Entities;

public class Tag : BaseSimpleEntity
{
    public string Name { get; set; } = null!;
    public int OwnerId {  get; set; }
}
