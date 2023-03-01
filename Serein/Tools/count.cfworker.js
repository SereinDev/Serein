addEventListener("fetch", (event) => event.respondWith(handle(event.request)));

async function handle(request) {
  try {
    const url = new URL(request.url.toLowerCase());
    if (url.protocol != 'https:') // 302 -> https
      return Response.redirect(request.url.replace(/^http:/, 'https:'));

    switch (url.pathname) {
      case '/heartbeat': // 心跳事件
        return heartbeat(url, request) || createResponse();

      case '/query': // 查询
        return createResponse((await COUNTKV.list()).keys || []);

      case '/':
        return createResponse('Welcome :)');

      case '/favicon.ico':
        return Response.redirect('https://serein.cc/assets/Serein.ico', 301);

      case '/getrequest':
        return createResponse(request);

      default:
        return createResponse('未知的操作', 406);
    }
  } catch (e) {
    console.error(e.stack);
    return createResponse(e.stack, 500);
  }
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

async function heartbeat(url, _request) {
  const guid = url.searchParams.get('guid');
  const version = url.searchParams.get('version');
  const type = url.searchParams.get('type');

  if (
    !/^[0-9a-f]{32}$/i.test(guid) || // guid可用性校验
    !/^v\d+\.\d+\.\d+$/.test(version) || // 版本号校验
    !['winform', 'console', 'wpf'].includes(type))// 版本类型校验
    return createResponse('参数错误', 400);

  let response = await write(guid, {
    version: version,
    type: type,
    isRunningServer: url.searchParams.get('isrunningserver') === '1',
    startTime: (Number)(url.searchParams.get('starttime')) || -1,
    region: null
  });

  console.log(response.headers);
  let result = await response.text();
  return createResponse(JSON.parse(result));
}

async function write(key_name, metadata) {
  let formdata = new FormData();
  formdata.append('value', '_');
  formdata.append('metadata', JSON.stringify(metadata));
  return await fetch(
    new Request(`https://api.cloudflare.com/client/v4/accounts/${AUTHORID}/storage/kv/namespaces/${KVID}/values/instance_${key_name.substring(0, 7)}?expiration_ttl=625`,
      {
        method: 'PUT',
        headers: {
          'X-Auth-Email': EMAIL,
          'Authorization': `Bearer ${APITOKEN}`,
        },
        body: formdata
      }));
}