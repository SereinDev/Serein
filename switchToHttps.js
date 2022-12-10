if (window.location.protocol.split(':')[0] == 'http' && window.location.hostname != '127.0.0.1') {
    window.location.href = location.href.replace('http', 'https');
}