启动后可进入swagger测试：
http://localhost:7080/swagger/index.html

# api/getBalance
可以在`appsettings.json`的`Config`-`TokenList`中配置要获取余额的Symbol，如：
```json
{
  "TokenList":[
    "ELF",
    "VOTE"
  ]
}
```
Usage：
```shell
http://localhost:7080/api/getBalance?address=2PdTR9GvuL4PgXbq4VoebbBRNWEPmAwzMmsCxiSsvP5gdymWBY
```
Response
```json
{
  "ELF": 39946480000,
  "VOTE": 0
}
```

# api/getBalanceBySymbol
需要再Request Body中输入symbol和address。
Usage：
```shell
curl -X POST "http://localhost:7080/api/getBalanceBySymbol" -H "accept: text/plain; v=1.0" -H "Content-Type: application/json; v=1.0" -d "{\"symbol\":\"ELF\",\"address\":\"2PdTR9GvuL4PgXbq4VoebbBRNWEPmAwzMmsCxiSsvP5gdymWBY\"}"
```
Response
```json
39946480000
```