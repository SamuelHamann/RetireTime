# The Design System: Editorial Earth & Refined Utility

## 1. Overview & Creative North Star
**The Creative North Star: "The Modern Tactile"**

This design system is a departure from the sterile, "tech-first" aesthetic of the last decade. It is an editorial-inspired framework that prioritizes human warmth and physical presence. By combining the grounded, sun-baked energy of **Terracotta** with the intellectual weight of **Slate**, we create an experience that feels like a high-end architectural journal. 

We break the "standard web" mold by utilizing **Newsreader’s** fluid, literary serifs against **Inter’s** precision. The design avoids rigid grids in favor of organic breathing room, using asymmetric white space and tonal layering to guide the eye. This is not a collection of boxes; it is a curated sequence of surfaces.

---

## 2. Colors & Tonal Architecture
The palette is rooted in a "Warm Cream" foundation, designed to reduce ocular strain and provide a sophisticated backdrop for our two primary anchors.

### The Palette
*   **Background (Base):** `#fdf9e9` (Warm Cream) — This is our canvas. Never use pure white.
*   **Primary (The Earth):** `#9A3412` (Terracotta) — Used for high-intent actions and brand moments.
*   **Secondary (The Slate):** `#334155` (Charcoal Slate) — Provides professional grounding and stability.
*   **Surface Containers:** Ranging from `surface_container_lowest` (#ffffff) to `surface_dim` (#dedacb).

### The "No-Line" Rule
Traditional 1px borders are strictly prohibited for sectioning. We define space through **Tonal Shifts**. To separate a sidebar from a main feed, transition from `surface` to `surface_container_low`. This creates a soft, architectural boundary that feels integrated rather than partitioned.

### Glass & Soul
To move beyond flat design, use **Glassmorphism** for floating navigation or overlays. 
*   **The Formula:** `surface_container_lowest` at 80% opacity + 20px Backdrop Blur.
*   **Signature Textures:** For Hero sections or Primary CTAs, apply a subtle linear gradient from `primary` (#781f00) to `primary_container` (#9a3412) at a 135-degree angle. This adds "visual soul" and depth.

---

## 3. Typography: The Editorial Voice
We use typography to establish a clear hierarchy between "The Story" (Display) and "The Utility" (Body).

*   **Display & Headlines (Newsreader):** Use `display-lg` (3.5rem) and `headline-md` (1.75rem) to create a sophisticated, editorial pace. The serif should feel "inked" on the cream background. Maintain a tight letter-spacing (-0.02em) for large headings.
*   **Body & Labels (Inter):** Inter handles the functional heavy lifting. Use `body-lg` (1rem) for long-form reading and `label-md` (0.75rem) for metadata. 
*   **The Contrast Rule:** Always pair a Newsreader headline with an Inter sub-headline. The tension between the organic serif and the geometric sans-serif is the hallmark of this system.

---

## 4. Elevation & Depth: Tonal Layering
We reject heavy drop shadows. Depth in this system is achieved through "Stacking."

*   **The Layering Principle:** Place a `surface_container_lowest` card (Pure White) on a `surface_container_low` (#f8f4e4) background. The contrast provides a "natural lift" without artificial effects.
*   **Ambient Shadows:** If an element must float (e.g., a Modal), use a shadow color tinted with the `on_surface` tone: `rgba(28, 28, 19, 0.06)` with a 40px blur and 12px Y-offset.
*   **The "Ghost Border" Fallback:** For accessibility in forms, use the `outline_variant` (#dec0b7) at **15% opacity**. It should be felt, not seen.

---

## 5. Components & Functional Elements

### Buttons
*   **Primary:** Full-pill shape (`9999px`). Background: `primary`. Text: `on_primary` (White). No border.
*   **Secondary:** Full-pill shape. Background: `secondary_container`. Text: `on_secondary_container`.
*   **Tertiary:** Text-only in `secondary`, using a slight `surface_variant` hover state.

### Cards & Lists
*   **The No-Divider Rule:** Explicitly forbid horizontal divider lines. Use `spacing-8` (2rem) of vertical white space or a subtle shift to `surface_container` to separate list items.
*   **Card Geometry:** Use `radius-xl` (3rem) for large containers to lean into the "Organic" brand pillar.

### Input Fields
*   **Surface-First Inputs:** Instead of a boxed outline, use a `surface_container_high` background with a `full` (9999px) rounded corner. Upon focus, transition the background to `surface_container_lowest` and add a 1px `primary` ghost border at 20% opacity.

### Featured Component: "The Gallery Card"
A signature component for this system: An image container with `radius-lg` (2rem), using a `tertiary_container` (Deep Slate Blue) overlay for captions, creating a grounded, high-contrast focal point.

---

## 6. Do’s and Don’ts

### Do:
*   **Do** embrace asymmetry. Center-align a Newsreader headline but left-align the Inter body text below it to create visual tension.
*   **Do** use `full` rounding (pill-shape) for all interactive elements like buttons, chips, and tags.
*   **Do** use the `primary_fixed_dim` color for subtle highlights in dark-themed sub-sections.

### Don't:
*   **Don't** use 100% black text. Always use `on_surface` (#1c1c13) to maintain the warmth of the palette.
*   **Don't** use sharp corners. Every element must feel eroded and soft, like a river stone.
*   **Don't** clutter the layout. If a screen feels "busy," increase the spacing token by two steps (e.g., move from `10` to `16`) and remove a container background.

### Accessibility Note:
While we prioritize aesthetics, ensure the `primary` terracotta (#9A3412) text always sits on `surface` backgrounds to maintain a 4.5:1 contrast ratio. For smaller labels, prefer `secondary` (Slate) for maximum legibility.