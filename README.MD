# DataManager & FTP server
Последние коммиты дополнили FTP сервер классами, работующими с базами данных. Если по порядку:
## ConfigManager.dll
Класс предоставляющий шаблонные методы для заполненния моделей конфигурации. Для удобства вынесен в отдельную либу.
## ConfigProvider.dll
Класс отвечающий за обработку файлов конфигов. В отличии от прежней версии дополнен методом заполнения объекта словарем свойств и их значений (для работы с сущностями получаемых из базы данных.
## DataAccessLayer.dll
Сущность, предоставляющая набор методов для доступа к данным из БД посредством ADO.NET и TransactionScopes.
## ServiceLayer.dll
Надстройка над DataAccessLayer, представляющая собой набор методов для более безопасной работы с данными посредством DataAccessLayer.
## DataManager
Сервис, сделанный в виде консольного приложения, что использует описанные выше сущности для считывания конфигурации и получения данных. После, серилизует их в xml файлы, и отправл ет последние на FTP сервер, который обрабатывает их по классическому сценарию. Делает три вида запросов: запрос всей таблицы Person, со всеми колонками из прилагающихся к ней таблиц: EmailAddress, Password, PersonPhone, Address. Запрос со слиянием таблиц посредством LEFT JOIN. Запрос аналогичный первому, но с разделением на пакеты. Получившиеся xml файлы в результате этих запросов можно посмотреть в папке DataManager/DataPulls.
## Model.dll
Библиотека классов, содержащая в себе модели конфигураций из предыдущей работы для FileManager'a, но дополнена моделями для опций DataManager'a.         

##Applications
По итогу решение будет выглядеть следующий образом:

Ниже пример работы DataManager&FileManager: