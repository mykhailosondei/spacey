import * as React from "react";
import type { SVGProps } from "react";
const SvgFamilyFriendly = (props: SVGProps<SVGSVGElement>) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    aria-hidden="true"
    style={{
      display: "block",
      height: 24,
      width: 24,
      fill: "currentcolor",
    }}
    viewBox="0 0 32 32"
    width="1em"
    height="1em"
    {...props}
  >
    <path d="M29 2v10a2 2 0 0 1-1.66 1.97L27 14h-1.03l2.73 10.18a43 43 0 0 0 1.68-1.23l1.25 1.56A24.9 24.9 0 0 1 16 30 24.9 24.9 0 0 1 .78 24.83l-.4-.31 1.25-1.56c.61.5 1.25.95 1.91 1.38L7.45 10c-1.2 0-2.31.88-2.7 2.04L3.7 16H1.62l1.15-4.3A5 5 0 0 1 7.37 8h10.7l.04-.22a7 7 0 0 1 6.15-5.74l.25-.02.25-.02H25zM17 20v2h-2v-2h-4.1l-1.86 6.93A23 23 0 0 0 16 28a23 23 0 0 0 7.2-1.15L21.37 20zm-5-10H9.44L5.32 25.37c.6.32 1.2.6 1.83.87L9.36 18H15v-2.13a4 4 0 0 1-3-3.67zm15-6h-2.18a5 5 0 0 0-4.8 4.58L20 8.8V12a4 4 0 0 1-3 3.87V18h5.9l2.18 8.14a23 23 0 0 0 1.84-.89L23.36 12 22.3 8h2.07l1.07 4H27zm-9 6h-4v2a2 2 0 0 0 2 2c1.05 0 2-.9 2-2z" />
  </svg>
);
export default SvgFamilyFriendly;
