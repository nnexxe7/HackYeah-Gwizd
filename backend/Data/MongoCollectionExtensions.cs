using System.Data;
using MongoDB.Driver;

namespace GWIZD.Core.Data;

public static class MongoCollectionExtensions
{
	public static void SaveDocument<TDocument, TKey>(this IMongoCollection<TDocument> collection, TDocument document, IClientSessionHandle session = null)
		where TDocument : IEntity<TKey>
	{
		if (document.Version == Guid.Empty)
		{
			bool created = false;
			try
			{
				document.Version = Guid.NewGuid();
				if (session == null)
				{
					collection.InsertOne(document);
				}
				else
				{
					collection.InsertOne(session, document);
				}
				created = true;
			}
			finally
			{
				if (!created) document.Version = Guid.Empty;
			}
		}
		else
		{
			Guid currentVersionTag = document.Version;
			document.Version = Guid.NewGuid();
			FilterDefinition<TDocument> q = Builders<TDocument>.Filter.Eq(n => n.Id, document.Id) &
			                                Builders<TDocument>.Filter.Eq(n => n.Version, currentVersionTag);
			bool updated = false;
			var opt = new FindOneAndReplaceOptions<TDocument> { IsUpsert = true };
			try
			{
				TDocument result = session == null ? collection.FindOneAndReplace(q, document, opt) : collection.FindOneAndReplace(session, q, document, opt);
				if (result == null)
				{
					throw new DBConcurrencyException($"Couldn't update document in collection {collection.CollectionNamespace.CollectionName} because newer version of document exists. {document.Id}/{document.Version}");
				}
				else
				{
					updated = true;
				}
			}
			finally
			{
				if (!updated) document.Version = currentVersionTag;
			}
		}
	}

	public static async Task SaveDocumentAsync<TDocument, TKey>(this IMongoCollection<TDocument> collection, TDocument document) where TDocument : IEntity<TKey>
	{
		if (document.Version == Guid.Empty)
		{
			bool created = false;
			try
			{
				document.Version = Guid.NewGuid();
				await collection.InsertOneAsync(document);
				created = true;
			}
			finally
			{
				if (!created) document.Version = Guid.Empty;
			}
		}
		else
		{
			Guid currentVersionTag = document.Version;
			document.Version = Guid.NewGuid();
			FilterDefinition<TDocument> q = Builders<TDocument>.Filter.Eq(n => n.Id, document.Id) &
			                                Builders<TDocument>.Filter.Eq(n => n.Version, currentVersionTag);
			bool updated = false;
			var opt = new FindOneAndReplaceOptions<TDocument> { IsUpsert = true };
			try
			{
				TDocument result = await collection.FindOneAndReplaceAsync(q, document, opt);
				if (result == null)
				{
					throw new DBConcurrencyException($"Couldn't update document in collection {collection.CollectionNamespace.CollectionName} because newer version of document exists. {document.Id}/{document.Version}");
				}
				else
				{
					updated = true;
				}
			}
			finally
			{
				if (!updated) document.Version = currentVersionTag;
			}
		}
	}
}