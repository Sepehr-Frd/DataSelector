using Nest;

namespace DataSelector.Model.Models;

[ElasticsearchType(RelationName = "question", IdProperty = nameof(Id))]
public class QuestionDocument : BaseDocument
{
    public string? Title { get; set; }

    public string? Description { get; set; }
}

