抽奖程序 Demo
sqlite 库

##### LockSite待处理 
> 1. 列表排序
> 2. 列表标题 超长换行
> 3. 列表删除 增加提示  删除后删除本行
> 4. 删除字段都改为 int 
> 5. 增加 检验次数字段 CheckCount
> 6. 增加 管理删除数据的页面
> 7. 列表数据绑定用户和登入
> 8. LP表 Id 字段上添加Key标识或者设置为主键
> 9. 数据库里需要添加 EditCount  UserId
> 10. 修改部分字段时            
      同一个字段多次赋值会添加多次到 _WriteFiled = Count = 12
> 11. 删除点否 收回当前行 

> 12. ExecuteScalar(sql,entity) 有bug 不需要传入sql,entity参数的
    var first = LockSqlite<Users, Skin>.Selec().Column((a, b) => new { Value = b.Value }).FromJoin(JoinType.Inner, (a, b) => a.SkinId == b.Id).Where( (a,b) => a.Id ==1 && a.UserName == "cc").ExecuteScalar(sql,entity)
  13. LockSqlite<Skin>.Cud.Inser(additem,true);   bug  
      未实现该方法=操作  SQLiteAdapter.Insert

  14. 未将对象引用到新实列
       int UserIds = 1;
                bool isSuccess = LockSqlite<Skin>.Cud.Update(s => {
                    s._IsWriteFiled = true; s.IsDel = 1;  },  w => w.Id == Id && w.UserId == UserIds);
					DapperSqlMaker.DapperExt.AnalysisExpression.GetMemberValue(System.Linq.Expressions.MemberExpression, System.String)

  15. Users Roles Skin 表 IsDel改为int类型

 
 16. 改为返回影响行数 bool isSuccess = DapperFuncs.New.Update<Skin>(s => {
                    s._IsWriteFiled = true; s.IsDel = 1;
                }
 17. 字体映射配置 .woff	application/x-font-woff	删除
                  .woff2	application/x-font-woff