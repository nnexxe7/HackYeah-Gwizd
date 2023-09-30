﻿namespace GWIZD.Model;

public class Attachment
{
	public AttachmentType Type { get; set; }
	public DateTime AddedAt { get; set; }
	public string Parameter1 { get; set; }
	public string Parameter2 { get; set; }
	public string SubmittedBy { get; set; }
}