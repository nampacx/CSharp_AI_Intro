# Polyglot Notebooks with Semantic Kernel and C#

Welcome to the repository for polyglot notebooks demonstrating the ease of getting started with Large Language Models (LLMs) using Semantic Kernel and C#. This repository contains examples and documentation to help you integrate cutting-edge AI capabilities into your applications.

## Working with the samples

The samples start with a number. I tried to start with simpler samples and than increase the complexity. The sample notebooks contain comments if new things are getting added  or if something is different the in the more simpler samples.


## Getting Started

To begin using the polyglot notebooks in this repository, you'll need to set up your environment with the necessary tools and extensions.

### Prerequisites

- Install the latest [.NET SDK](https://dotnet.microsoft.com/en-us/download).
- Install the [Visual Studio Code (VS Code)](https://code.visualstudio.com/Download).
- Add the [Polyglot Notebooks extension to VS Code](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.dotnet-interactive-vscode).
- [Azure OpenAI ](https://learn.microsoft.com/en-us/azure/ai-services/openai/) deployed to region which supports GPT4o and dall-e-3, can be chacked [here](https://learn.microsoft.com/en-us/azure/ai-services/openai/concepts/models#model-summary-table-and-region-availability)

The following models are used:
- [GPT4o](https://learn.microsoft.com/en-us/azure/ai-services/openai/concepts/models#gpt-4o-and-gpt-4-turbo)
- [Dall-e-3](https://learn.microsoft.com/en-us/azure/ai-services/openai/concepts/models#dall-e-models)
- [text-embedding-ada-002](https://learn.microsoft.com/en-us/azure/ai-services/openai/concepts/models#embeddings-models)

### Cofigure Azure OpenAi Settings

Update the [setting file](https://github.com/nampacx/CSharp_AI_Intro/blob/main/src/config/settings.json%20-%20sample)

```json
{
  "type": "azure", //azure or openai 
  "model": "", //for azure openai the deployment name
  "texttoimagemodel": "",//deployment name of the model used for text to image
  "endpoint": "", //endpoint of the azure openai serivce
  "apikey": "", //apikey from the azure openai serivce
  "aisearchendpoint": "", // azure ai search endpoint
  "aisearchkey": "" //azure ai search key
}
```

### Using Polyglot Notebooks

Open the `.ipynb` files in VS Code with the Polyglot Notebooks extension to start experimenting with multiple programming languages in a single notebook.

### Semantic Kernel Integration

Follow the documentation to integrate Semantic Kernel into your C# applications, enabling seamless orchestration of AI services.

### Local models

[Simple-Chat-Local](https://github.com/nampacx/CSharp_AI_Intro/blob/main/src/07-Simple-Chat-Local.ipynb) uses a model hosted on the local machine.

For running the model [Ollama](https://ollama.com/) is used.

After the installation, all we have to do is start the application and use the following cli command:
```bash
ollama run phi3:mini
```

This will download the model an run it locally.

Other Ollama supports many other [models](https://ollama.com/library).

## Resources

- [Visual Studio Polyglot Extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.dotnet-interactive-vscode) - Enhance your notebook experience in VS Code with support for multiple languages.
- [Semantic Kernel](https://github.com/microsoft/semantic-kernel) - An open-source SDK for integrating LLMs with conventional programming languages.
- [Scott and Mark learn AI](https://youtu.be/KKWPSkYN3vw?si=vhPOOex1L-voTsdA)

## Contributing

Contributions to this repository are welcome.

## License

This project is licensed under the MIT License.


## Acknowledgments

- Thanks to all the contributors who have helped with the development of these notebooks.
- Special thanks to the Semantic Kernel team for providing the tools to integrate AI services.
