using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Messaging;

namespace MQMQ
{
    internal class Program
    {
        private static MessageQueue _queue;
        private static MessageQueueTransaction _tx;
        private static Stopwatch _stopwatch;

        private static void Main(string[] args)
        {
            //CreateQueue();
            //SendANonDurableMessage();
            //SendADurableMessage();
            //SendATransactionalMessage();

            PurgeQueues();
            
            SendADurableMessage();

            //Console.WriteLine("Elapsed time :: " + _stopwatch.ElapsedMilliseconds);
        }

        private static void PurgeQueues()
        {
            var queues = new List<string>
            {
                @".\private$\transactional-queue",
                @".\private$\durable-queue",
                @".\private$\default-queue",
                @".\private$\secure-queue"
            };
            queues.ForEach(x =>
            {
                var queue = new MessageQueue(x);
                queue.Purge();
            });
        }


        private static void SendATransactionalMessage()
        {
            _queue = new MessageQueue(@".\private$\transactional-queue");
            _stopwatch = Stopwatch.StartNew();
            _tx = new MessageQueueTransaction();
            _tx.Begin();
            for (var i = 0; i < 1000; i++)
            {
                
                _queue.Send("This is my message number " + i, _tx);
            }
            _tx.Commit();
        }

        private static void SendADurableMessage()
        {
            _queue = new MessageQueue(@".\private$\durable-queue")
            {
                DefaultPropertiesToSend =
                {
                    Recoverable = true
                }
               
            };

            _stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000; i++)
            {
                Console.ReadKey();
                _queue.Send("This is my message number " + i);
            }
        }

        private static void SendANonDurableMessage()
        {
            _queue = new MessageQueue(@".\private$\default-queue");
            _stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 1000; i++)
                _queue.Send("This is my message number " + i);
        }

        private static void CreateQueue()
        {
            var queueName = @".\private$\secure-queue";
            var queueNameDurable = @".\private$\durable-queue";
            MessageQueue.Create(queueName, true);
            var secureQueue = new MessageQueue(queueName)
            {
                Authenticate = true,
                EncryptionRequired = EncryptionRequired.Body
            };
            MessageQueue.Create(queueNameDurable);
        }
    }
}