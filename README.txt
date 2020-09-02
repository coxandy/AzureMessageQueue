
Hi

We want to implement a an Azure migration exercise.

In this solution we have two .Net Core Console applications simulating a basic publisher-subscriber mechanism using the file system.

We would like you to:

1) Convert/Migrate the MessagePublisher console into a Web App that can be published into Azure (you don't have to actually deploy it into Azure)
2) The MessagePublisher should receive calculation requests from a web request
3) Implement the message Queue using Azure Storage Queue and/or Azure Storage Blob Containers for large data
4) Convert the listener so it will consume the messages from the Queue
5) How would you do to make it as easy as possible to switch to a new Message Queue solution in the future?


References:
You can use Azure Storage Emulator or use this Connection String for test purposes:
DefaultEndpointsProtocol=https;AccountName=lbgazuretest;AccountKey=ZV1uWrheTKv9xPh68TTzxNYypdHfqNQEmhVSWdrMVZnU5Iml7F218g62GPkw2Sg9cXtZ4zxLBid8naBVbUYjFg==;EndpointSuffix=core.windows.net

You are free to create new Azure Storage Queues, but you can also use this one already available: test-azure-01

IncomingRequestTest.csv is a sample file for incoming messages being received in the file system