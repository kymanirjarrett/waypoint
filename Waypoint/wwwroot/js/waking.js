// Counts down on the "Waking up" page, then reloads the page the visitor asked for.
// External file because the Content-Security-Policy blocks inline scripts.
(function () {
  var box = document.querySelector('[data-waking-countdown]');
  if (!box) return;
  var label = box.querySelector('[data-waking-seconds]');
  var seconds = parseInt(box.getAttribute('data-seconds'), 10) || 60;
  var url = box.getAttribute('data-url') || '/';
  // Only follow same-site paths ("/x"), never "//host" or full URLs.
  if (url.charAt(0) !== '/' || url.charAt(1) === '/' || url.charAt(1) === '\\') url = '/';

  var timer = setInterval(function () {
    seconds -= 1;
    if (seconds <= 0) {
      clearInterval(timer);
      label.textContent = '0';
      window.location.replace(url);
      return;
    }
    label.textContent = String(seconds);
  }, 1000);
})();
