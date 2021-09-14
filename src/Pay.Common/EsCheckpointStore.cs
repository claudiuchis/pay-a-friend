using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using Eventuous.Subscriptions;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace Pay.Common
{
    public class EsCheckpointStore : ICheckpointStore
    {
        const string CheckpointStreamPrefix = "checkpoint:";
        Type CheckpointEventType = Type.GetType("Checkpoint");
        readonly EventStoreClient _connection;
        readonly ILogger<EsCheckpointStore> _log;

        public EsCheckpointStore(
            EventStoreClient connection,
            ILogger<EsCheckpointStore> logger)
        {
            _connection = connection;
            _log = logger;
        }

        public async ValueTask<Checkpoint> GetLastCheckpoint(
            string checkpointId, 
            CancellationToken cancellationToken = default
        )
        {
            Checkpoint checkpoint;
            var stream = CheckpointStreamPrefix + checkpointId;
            try 
            {
                var read = _connection
                    .ReadStreamAsync(Direction.Backwards, stream, StreamPosition.End, 1);
                var resolvedEvents = await read.ToArrayAsync(cancellationToken);
                ResolvedEvent eventData = resolvedEvents.FirstOrDefault();

                var jsonData = Encoding.UTF8.GetString(eventData.Event.Data.ToArray());
                checkpoint = JsonConvert.DeserializeObject<Checkpoint>(jsonData);
            }
            catch (StreamNotFoundException) {
                checkpoint = new Checkpoint(checkpointId, null);
            }

            return checkpoint;
        }

        public async ValueTask<Checkpoint> StoreCheckpoint(
            Checkpoint checkpoint, 
            CancellationToken cancellationToken = default)
        {
            var stream = CheckpointStreamPrefix + checkpoint.Id;
            var eventData = new EventData(
                Uuid.NewUuid(),
                "Checkpoint",
                Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(checkpoint)),
                null
            );

            Task<IWriteResult> resultTask;
            resultTask = _connection.AppendToStreamAsync(
                stream, 
                StreamState.Any, 
                new[] {eventData}, 
                cancellationToken: cancellationToken);

            await resultTask.ConfigureAwait(false);

            if (checkpoint.Position == null)
                await SetStreamMaxCount(stream);

            return checkpoint;
        }

        async Task SetStreamMaxCount(string stream)
        {
            var metadata = await _connection.GetStreamMetadataAsync(stream);

            if (!metadata.Metadata.MaxCount.HasValue)
                await _connection.SetStreamMetadataAsync(
                    stream, 
                    StreamState.Any,
                    new StreamMetadata(1)
                );
        }

    }
}