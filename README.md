## 🌦️ MauiAppTempoAgora

Aplicativo desenvolvido em .NET MAUI para consulta de informações climáticas em tempo real através da API OpenWeatherMap. O sistema permite pesquisar o clima de uma cidade, visualizar dados meteorológicos e armazenar o histórico das consultas em um banco SQLite.

## Funcionalidades
Consulta do clima por cidade;
Exibição de temperatura, sensação térmica e descrição do clima;
Visualização de latitude e longitude;
Exibição de mapa da localização pesquisada;
Armazenamento do histórico de consultas;
Filtros por cidade e período.
## Tecnologias Utilizadas
.NET MAUI
C#
XAML
SQLite
OpenWeatherMap API
MVVM
## Estrutura do Projeto
Models: representam os dados climáticos e consultas.
Services: realizam a comunicação com a API.
ViewModels: controlam a lógica da aplicação.
Views: interfaces de consulta e histórico.
## Funcionamento
O usuário informa uma cidade.
O aplicativo consulta a API OpenWeatherMap.
Os dados climáticos são exibidos na tela.
A consulta é salva automaticamente no SQLite.
O histórico pode ser visualizado posteriormente.
## Objetivo

Aplicar conceitos de desenvolvimento mobile com .NET MAUI, consumo de APIs REST, persistência de dados com SQLite e arquitetura MVVM.
