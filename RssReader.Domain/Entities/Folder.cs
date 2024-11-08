﻿using RssReader.Domain.Common;

namespace RssReader.Domain.Entities;

public class Folder : BaseSimpleEntity<int>
{
    public string Name { get; set; } = null!;
    public int? ParentId { get; set; }
    public int OwnerId { get; set; }
}
