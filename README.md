# poke

Command-line message publisher for RabbitMQ

## Overview

**Poke** is a command-line application designed to send arbitrary messages to RabbitMQ exchanges and queues. This tool provides a quick and flexible way to stimulate various consumers and perform stress tests. Its scriptable nature makes it ideal for integration into automation pipelines or testing workflows.

## Features

- Send custom messages to RabbitMQ exchanges or queues.
- Define message payloads and configure reply queues.
- Configure connection settings directly from the command line or a configuration file.
- Perform stress tests by controlling wait times and responses.
- Scriptable for seamless integration into automated testing or CI/CD pipelines.

## Installation

1. Ensure you have the .NET SDK installed on your machine. You can download it from [Microsoft .NET](https://dotnet.microsoft.com/download).
2. Clone the repository or download the application binaries.
3. Build or run the application:
   ```bash
   dotnet build
   ```

   Or, if you have the binaries:
   ```bash
   ./poke
   ```

## Usage

The application is executed from the command line with various options for customization. Below are examples of common usage scenarios.

### Command Syntax

```bash
poke [options]
```

### Options

| Option                  | Description                                                             |
|-------------------------|-------------------------------------------------------------------------|
| `--config`             | Path to a JSON configuration file                                        |
| `--connection`         | RabbitMQ connection string                                               |
| `--type`               | The queue type to publish the message to                                 |
| `--message`            | The message payload to send                                              |
| `--exchange`           | Name of the exchange to publish the message to                           |
| `--replyto`            | The queue where a response will be read                                  |
| `--wait`               | Time to wait for a response, in seconds                                  |

### Examples

#### Sending a Simple Message

```bash
poke --connection amqp://guest:guest@localhost:5672 --exchange my-exchange --type queue --message "Hello, Poke!"
```

#### Sending a Message with a Reply Queue

```bash
poke --connection amqp://guest:guest@localhost:5672 --exchange logs --type queue --message "Request Data" --replyto response-queue --wait 5000
```

#### Using a Configuration File

Create a configuration file `config.json`:

```json
{
  "connection": "amqp://guest:guest@localhost:5672",
  "exchange": "test-exchange",
  "type": "queue",
  "message": "Hello from config!",
  "replyto": "response-queue",
  "wait": 5
}
```

Run the command:

```bash
poke --config config.json
```

## Configuration

### Configuration File Format

The configuration file should be in JSON format. Below is an example:

```json
{
  "Connection": "amqp://guest:guest@localhost:5672",
  "Exchange": "default-exchange",
  "TypeQueue": "queue",
  "Message": "default message",
  "ReplyTo": "default-response-queue",
  "WaitSeconds": 1
}
```

## Contribution

Contributions are welcome! Feel free to open issues or submit pull requests for enhancements or bug fixes.

## License

This project is licensed under the MIT License. See the LICENSE file for more details.

