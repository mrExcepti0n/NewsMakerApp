# DataSearchAngularApp

Новостной портал написанный на Angular + .net core с использованием docker. 

Присутствует возможность добавлять и редактироваться посты.

Добавлен модуль комментирования, взаимодествие клиентского приложения с сервером осуществляется через SignalR(WebSockets).

Для гибкого взаимодествия интеграционных процессов используется брокер сообщений RabbitMQ (RabbitMQClient) и библиотека MediatR.

Для быстрого поиска новостной информации используется ElasticSearch.

Для логирования используется serilog + elasticsearch + kibina.

Простая диаграмма с группировкой по типу сообщений:
![диаграмма](https://github.com/mrExcepti0n/NewsMakerApp/raw/master/.images/kibana-diagram.png)

Для доккументации используется swagger и библиотека swashbuckle.
