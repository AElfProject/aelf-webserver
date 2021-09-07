Use swagger to test after launching:
http://localhost:7080/swagger/index.html

# api/getBalance
Config symbol list in `Config`-`TokenList` of `appsettings.json`
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
Need to input `symbol` & `address` to Request Body.
Usage：
```shell
curl -X POST "http://localhost:7080/api/getBalanceBySymbol" -H "accept: text/plain; v=1.0" -H "Content-Type: application/json; v=1.0" -d "{\"symbol\":\"ELF\",\"address\":\"2PdTR9GvuL4PgXbq4VoebbBRNWEPmAwzMmsCxiSsvP5gdymWBY\"}"
```
Response
```json
39946480000
```