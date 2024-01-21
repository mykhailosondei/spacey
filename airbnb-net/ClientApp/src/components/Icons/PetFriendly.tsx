import * as React from "react";
import type { SVGProps } from "react";
const SvgPetFriendly = (props: SVGProps<SVGSVGElement>) => (
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
    <path d="M13.7 13.93a4 4 0 0 1 5.28.6l.29.37 4.77 6.75a4 4 0 0 1 .6 3.34 4 4 0 0 1-4.5 2.91l-.4-.08-3.48-.93a1 1 0 0 0-.52 0l-3.47.93a4 4 0 0 1-2.94-.35l-.4-.25a4 4 0 0 1-1.2-5.2l.23-.37 4.77-6.75a4 4 0 0 1 .96-.97zm3.75 1.9a2 2 0 0 0-2.98.08l-.1.14-4.84 6.86a2 2 0 0 0 2.05 3.02l.17-.04 4-1.07a1 1 0 0 1 .5 0l3.97 1.06.15.04a2 2 0 0 0 2.13-2.97l-4.95-7.01zM27 12a4 4 0 1 1 0 8 4 4 0 0 1 0-8M5 12a4 4 0 1 1 0 8 4 4 0 0 1 0-8m22 2a2 2 0 1 0 0 4 2 2 0 0 0 0-4M5 14a2 2 0 1 0 0 4 2 2 0 0 0 0-4m6-10a4 4 0 1 1 0 8 4 4 0 0 1 0-8m10 0a4 4 0 1 1 0 8 4 4 0 0 1 0-8M11 6a2 2 0 1 0 0 4 2 2 0 0 0 0-4m10 0a2 2 0 1 0 0 4 2 2 0 0 0 0-4" />
  </svg>
);
export default SvgPetFriendly;
