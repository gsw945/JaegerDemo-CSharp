# OpenTracing Jaeger C# 示例

## 操作记录
1. 从 `https://github.com/jaegertracing/jaeger/releases/` 下载所需版本的jaeger
2. 解压后，运行`jaeger-all-in-one`
3. 浏览器打开 `http://127.0.0.1:16686/` 可查看 Traces
4. 按照 `https://github.com/yurishkuro/opentracing-tutorial/tree/master/csharp`, 在C#中接入Jaeger
	- 关键代码(指定使用`GrpcSender`, 否则会默认使用`NoopSender`, 不会把数据发送到Jaeger)
		- `RegisterSenderFactory<GrpcSenderFactory>()`
		- `.WithEndpoint("127.0.0.1:14250")`

## 参考
- [Getting Started - Get up and running with Jaeger in your local environment](https://www.jaegertracing.io/docs/1.25/getting-started/)
- [OpenTracing Tutorial - C#](https://github.com/yurishkuro/opentracing-tutorial/tree/master/csharp)