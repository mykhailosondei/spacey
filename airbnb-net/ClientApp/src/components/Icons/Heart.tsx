import * as React from "react";
import type { SVGProps } from "react";
const SvgHeart = (props: SVGProps<SVGSVGElement>) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    aria-hidden="true"
    style={{
      display: "block",
      height: 16,
      width: 16,
      stroke: "currentcolor",
      strokeWidth: 2,
      overflow: "visible",
    }}
    viewBox="0 0 32 32"
    width="1em"
    height="1em"
    fill={props.fill || "none"}
    {...props}
  >
    <path d="M16 28c7-4.73 14-10 14-17a6.98 6.98 0 0 0-7-7c-1.8 0-3.58.68-4.95 2.05L16 8.1l-2.05-2.05a6.98 6.98 0 0 0-9.9 0A6.98 6.98 0 0 0 2 11c0 7 7 12.27 14 17z" />
  </svg>
);
export default SvgHeart;
