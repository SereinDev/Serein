export default {
  async fetch(request, env) {
    try {
      const url = new URL(request.url.toLowerCase());

      if (url.protocol != 'https:') // 302 -> https
        return Response.redirect(request.url.replace(/^http:/, 'https:'));

      switch (url.pathname) {

        case '/heartbeat': // 心跳事件
          const guid = url.searchParams.get('guid');
          const version = url.searchParams.get('version');
          const type = url.searchParams.get('type');
          const isRunningServer = url.searchParams.get('isrunningserver') === '1';
          const runTime = (Number)(url.searchParams.get('runtime')) || -1;
          const region = request.cf.region || null;

          if (
            !/^\w{32}$/.test(guid) || // guid可用性校验
            !/^v\d+\.\d+\.\d+$/.test(version) || // 版本号校验
            !['winform', 'console', 'wpf'].includes(type))// 版本类型校验
            return createResponse('参数错误', 400);

          await env.COUNTKV.put(
            'instance_' + guid.substring(0, 7), // 键名
            Date.now().toString(), // 更新时间
            {
              expirationTtl: 625, // 过期时间
              metadata: { // 元数据
                version: version, // 版本
                type: type, // 类型
                isRunningServer: isRunningServer, // 服务器是否正在运行
                runTime: runTime, // Serein运行时间（分钟）
                region: region // 区域
              }
            })
          return createResponse();

        case '/query': // 查询
          const list = await env.COUNTKV.list({ prefix: 'instance_' }); // 列出kv
          return createResponse(list.keys || []);

        case '/':
          return createResponse('Welcome :)');

        case '/favicon.ico':
          return Response.redirect('https://serein.cc/assets/Serein.png', 301);

        case '/getrequest':
          return createResponse(request);

        default:
          return createResponse('未知的操作', 406);
      }
    } catch (e) {
      console.error(e.stack);
      return createResponse(e.stack, 500);
    }

    function createResponse(data = null, status = 200) {
      return new Response(
        JSON.stringify({
          code: status,
          data: data
        }),
        {
          status: status,
          headers: { // 允许跨源
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Credentials': 'true'
          }
        }
      );
    }
  }
}