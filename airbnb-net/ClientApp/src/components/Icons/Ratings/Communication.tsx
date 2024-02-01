import * as React from "react";
import type { SVGProps } from "react";
const SvgCommunication = (props: SVGProps<SVGSVGElement>) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    aria-hidden="true"
    style={{
      display: "block",
      fill: "none",
      height: 32,
      width: 32,
      stroke: "currentcolor",
      strokeWidth: 2,
      overflow: "visible",
    }}
    viewBox="0 0 32 32"
    width="1em"
    height="1em"
    {...props}
  >
    <path d="M26 3a4 4 0 0 1 4 4v14a4 4 0 0 1-4 4h-6.32L16 29.5 12.32 25H6a4 4 0 0 1-4-4V7a4 4 0 0 1 4-4z" />
  </svg>
);
export default SvgCommunication;
