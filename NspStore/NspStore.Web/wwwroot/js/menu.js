// wwwroot/js/menu.js
(() => {
    const toggle = document.getElementById('menuToggle');
    const menu = document.getElementById('menu');

    if (!toggle || !menu) return;

    const OPEN_CLASS = 'navbar__menu--open';

    const setExpanded = (isOpen) => {
        toggle.setAttribute('aria-expanded', String(isOpen));
    };

    toggle.addEventListener('click', () => {
        const isOpen = menu.classList.toggle(OPEN_CLASS);
        setExpanded(isOpen);
    });

    // Закрыть меню по ESC
    document.addEventListener('keydown', (e) => {
        if (e.key === 'Escape' && menu.classList.contains(OPEN_CLASS)) {
            menu.classList.remove(OPEN_CLASS);
            setExpanded(false);
        }
    });
})();
