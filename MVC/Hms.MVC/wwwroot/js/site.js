// ══════════════════════════════════════════════════════════════
// HMS Site JavaScript — Theme toggle, Sidebar, Utilities
// ══════════════════════════════════════════════════════════════

// ── Theme Toggle ─────────────────────────────────────────────
const ThemeManager = {
    init() {
        const saved = localStorage.getItem('hms-theme') || 'dark';
        this.setTheme(saved);

        document.querySelectorAll('.theme-toggle').forEach(btn => {
            btn.addEventListener('click', () => this.toggle());
        });
    },

    setTheme(theme) {
        document.documentElement.setAttribute('data-theme', theme);
        localStorage.setItem('hms-theme', theme);
        this.updateIcons(theme);
    },

    toggle() {
        const current = document.documentElement.getAttribute('data-theme') || 'dark';
        this.setTheme(current === 'dark' ? 'light' : 'dark');
    },

    updateIcons(theme) {
        document.querySelectorAll('.theme-toggle').forEach(btn => {
            btn.innerHTML = theme === 'dark' ? '☀️' : '🌙';
            btn.title = theme === 'dark' ? 'Switch to Light Mode' : 'Switch to Dark Mode';
        });
    }
};

// ── Sidebar Toggle (Mobile) ─────────────────────────────────
const SidebarManager = {
    init() {
        const toggle = document.getElementById('sidebarToggle');
        const sidebar = document.getElementById('sidebar');
        const overlay = document.getElementById('sidebarOverlay');

        if (toggle && sidebar) {
            toggle.addEventListener('click', () => {
                sidebar.classList.toggle('open');
                overlay?.classList.toggle('active');
            });

            overlay?.addEventListener('click', () => {
                sidebar.classList.remove('open');
                overlay.classList.remove('active');
            });
        }
    }
};

// ── Active Nav Link ──────────────────────────────────────────
const NavManager = {
    init() {
        const path = window.location.pathname.toLowerCase();
        document.querySelectorAll('.sidebar .nav-link').forEach(link => {
            const href = link.getAttribute('href')?.toLowerCase();
            if (href && path === href) {
                link.classList.add('active');
            } else if (href && href !== '/' && path.startsWith(href)) {
                link.classList.add('active');
            }
        });
    }
};

// ── Initialize ───────────────────────────────────────────────
document.addEventListener('DOMContentLoaded', () => {
    ThemeManager.init();
    SidebarManager.init();
    NavManager.init();
});
